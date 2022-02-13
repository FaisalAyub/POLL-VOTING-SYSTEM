import { Component, ViewChild, Injector, Output, EventEmitter} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { DirectMessagesServiceProxy, CreateOrEditDirectMessageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { UserLookupTableModalComponent } from './user-lookup-table-modal.component';


@Component({
    selector: 'createOrEditDirectMessageModal',
    templateUrl: './create-or-edit-directMessage-modal.component.html'
})
export class CreateOrEditDirectMessageModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', {static: true}) modal: ModalDirective;
    @ViewChild('userLookupTableModal', {static: true}) userLookupTableModal: UserLookupTableModalComponent;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    directMessage: CreateOrEditDirectMessageDto = new CreateOrEditDirectMessageDto();

    userName = '';


    constructor(
        injector: Injector,
        private _directMessagesServiceProxy: DirectMessagesServiceProxy
    ) {
        super(injector);
    }

    show(directMessageId?: number,receiverId?:number): void {

        if (!directMessageId) {
            this.directMessage = new CreateOrEditDirectMessageDto();
            this.directMessage.id = directMessageId;
            this.directMessage.userId = receiverId;
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._directMessagesServiceProxy.getDirectMessageForEdit(directMessageId).subscribe(result => {
                this.directMessage = result.directMessage;

                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
    }

    save(): void {
            this.saving = true;

			
            this._directMessagesServiceProxy.createOrEdit(this.directMessage)
             .pipe(finalize(() => { this.saving = false;}))
             .subscribe(() => {
                this.notify.info('Sent successfully');
                this.close();
                this.modalSave.emit(null);
             });
    }

        openSelectUserModal() {
        this.userLookupTableModal.id = this.directMessage.userId;
        this.userLookupTableModal.displayName = this.userName;
        this.userLookupTableModal.show();
    }


        setUserIdNull() {
        this.directMessage.userId = null;
        this.userName = '';
    }


        getNewUserId() {
        this.directMessage.userId = this.userLookupTableModal.id;
        this.userName = this.userLookupTableModal.displayName;
    }


    close(): void {

        this.active = false;
        this.modal.hide();
    }
}
