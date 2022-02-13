import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { PollsServiceProxy, CreateOrEditPollDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';


@Component({
    selector: 'createOrEditPollModal',
    templateUrl: './create-or-edit-poll-modal.component.html'
})
export class CreateOrEditPollModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal') modal: ModalDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    poll: CreateOrEditPollDto = new CreateOrEditPollDto();



    constructor(
        injector: Injector,
        private _pollsServiceProxy: PollsServiceProxy
    ) {
        super(injector);
    }

    show(pollId?: number): void {

        if (!pollId) {
            this.poll = new CreateOrEditPollDto();
            this.poll.id = pollId;

            this.active = true;
            this.modal.show();
        } else {
            this._pollsServiceProxy.getPollForEdit(pollId).subscribe(result => {
                this.poll = result.poll;


                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._pollsServiceProxy.createOrEdit(this.poll)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
             });
    }







    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
