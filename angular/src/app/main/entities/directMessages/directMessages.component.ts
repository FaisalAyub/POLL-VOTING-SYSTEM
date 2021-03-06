import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DirectMessagesServiceProxy, DirectMessageDto  } from '@shared/service-proxies/service-proxies';
import { NotifyService } from '@abp/notify/notify.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditDirectMessageModalComponent } from './create-or-edit-directMessage-modal.component';
import { ViewDirectMessageModalComponent } from './view-directMessage-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/components/table/table';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
    templateUrl: './directMessages.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class DirectMessagesComponent extends AppComponentBase {

    @ViewChild('createOrEditDirectMessageModal', {static: true}) createOrEditDirectMessageModal: CreateOrEditDirectMessageModalComponent;
    @ViewChild('viewDirectMessageModalComponent', {static: true}) viewDirectMessageModal: ViewDirectMessageModalComponent;
    @ViewChild('dataTable', {static: true}) dataTable: Table;
    @ViewChild('paginator', {static: true}) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    textFilter = '';
        userNameFilter = '';




    constructor(
        injector: Injector,
        private _directMessagesServiceProxy: DirectMessagesServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
    }

    getDirectMessages(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            return;
        }

        this.primengTableHelper.showLoadingIndicator();

        this._directMessagesServiceProxy.getAll(
            this.filterText,
            this.textFilter,
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

    createDirectMessage(): void {
        this.createOrEditDirectMessageModal.show();
    }

    deleteDirectMessage(directMessage: DirectMessageDto): void {
        this.message.confirm(
            '',
            (isConfirmed) => {
                if (isConfirmed) {
                    this._directMessagesServiceProxy.delete(directMessage.id)
                        .subscribe(() => {
                            this.reloadPage();
                            this.notify.success(this.l('SuccessfullyDeleted'));
                        });
                }
            }
        );
    }

    exportToExcel(): void {
        this._directMessagesServiceProxy.getDirectMessagesToExcel(
        this.filterText,
            this.textFilter,
            this.userNameFilter,
        )
        .subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
         });
    }
}
