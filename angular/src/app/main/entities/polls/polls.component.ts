import { Component, Injector, ViewEncapsulation, ViewChild, HostListener } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PollsServiceProxy, PollDto, VotesServiceProxy, CreateOrEditVoteDto, CommentsServiceProxy  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditPollModalComponent } from './create-or-edit-poll-modal.component';
import { ViewPollModalComponent } from './view-poll-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';
import { CreateOrEditDirectMessageModalComponent } from '../directMessages/create-or-edit-directMessage-modal.component';

@Component({
    templateUrl: './polls.component.html', 
     styleUrls: [
        './polls.component.scss'
    ],
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class PollsComponent extends AppComponentBase {

    @ViewChild('createOrEditPollModal', {static: true}) createOrEditPollModal: CreateOrEditPollModalComponent;
    @ViewChild('viewPollModalComponent', {static: true}) viewPollModal: ViewPollModalComponent;
    @ViewChild('dataTable', {static: true}) dataTable: Table;
    @ViewChild('paginator', {static: true}) paginator: Paginator;
    @ViewChild('createOrEditDirectMessageModal', {static: true}) createOrEditDirectMessageModal: CreateOrEditDirectMessageModalComponent;

    advancedFiltersAreShown = false;
    filterText = '';
    titleFilter = '';
    option1Filter = '';
    option2Filter = '';
    option3Filter = '';
    option4Filter = '';
        userNameFilter = '';




    constructor(
        injector: Injector,
        private _pollsServiceProxy: PollsServiceProxy,
        private _votesServiceProxy: VotesServiceProxy,
        private _commentsServiceProxy: CommentsServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    createDirectMessage(userId): void {
        this.createOrEditDirectMessageModal.show(null,userId);
    }

    getPolls(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._pollsServiceProxy.getAll(
            this.filterText,
            this.titleFilter,
            this.option1Filter,
            this.option2Filter,
            this.option3Filter,
            this.option4Filter,
            this.userNameFilter,
            this.primengTableHelper.getSorting(this.dataTable),
            this.primengTableHelper.getSkipCount(this.paginator, event),
            this.primengTableHelper.getMaxResultCount(this.paginator, event)
        ).subscribe(result => {
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            this.primengTableHelper.records = result.items;
            this.primengTableHelper.hideLoadingIndicator();
        });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createPoll(): void {
        this.createOrEditPollModal.show();
    }

    deletePoll(poll: PollDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._pollsServiceProxy.delete(poll.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._pollsServiceProxy.getPollsToExcel(
        this.filterText,
            this.titleFilter,
            this.option1Filter,
            this.option2Filter,
            this.option3Filter,
            this.option4Filter,
            this.userNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
 
    checkPollEvent(record,option,checkedOption,e){
         
        console.log(record)
        console.log(this.primengTableHelper.records);
      let item=  this.primengTableHelper.records.find(x=>x.poll.id==record.poll.id);
      if(item){
          item.poll.checkedOption=checkedOption;
        //   if(checkedOption==1){
        //       item.poll.count1+=1;
        //   }else if(checkedOption==2){
        //     item.poll.count2+=1;
        //     }else if(checkedOption==3){
        //         item.poll.count3+=1;
        //     }else if(checkedOption==4){
        //         item.poll.count4+=1;
        //     }
      }
      let requestItem:any={pollId:record.poll.id,selectedOption:checkedOption};
        this._votesServiceProxy.createOrEdit(requestItem).subscribe((res)=>{
            // this.reloadPage();
            this.notify.info('Vote casted successfully');
           
          });
    }

    message:any='';
    sendMessage(pollId){
        if(this.message){
        let item:any={pollId:pollId,text:this.message};
        this._commentsServiceProxy.createOrEdit(item).subscribe((res)=>{
            this.notify.info('Comment added successfully');
            this.message='';
        })
    }
    }
    comments:any=[];
    pollId:any=0;
    getComments(pollId){ 
        this._commentsServiceProxy.getAll('','','','',pollId,'',0,10).subscribe((res)=>{
          this.comments= res.items;
          if(this.comments.length>0){
            this.pollId=pollId;
          }
        })
    }
}
