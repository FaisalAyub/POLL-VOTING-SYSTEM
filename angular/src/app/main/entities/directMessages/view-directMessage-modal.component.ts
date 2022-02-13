import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { GetDirectMessageForViewDto, DirectMessageDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewDirectMessageModal',
    templateUrl: './view-directMessage-modal.component.html'
})
export class ViewDirectMessageModalComponent extends AppComponentBase {

    @ViewChild('createOrEditModal', {static: true}) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetDirectMessageForViewDto;


    constructor(
        injector: Injector
    ) {
        super(injector);
        this.item = new GetDirectMessageForViewDto();
        this.item.directMessage = new DirectMessageDto();
    }

    show(item: GetDirectMessageForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
