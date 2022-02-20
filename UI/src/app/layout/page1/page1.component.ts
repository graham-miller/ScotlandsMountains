import { Component, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-page1',
  templateUrl: './page1.component.html',
  styleUrls: ['./page1.component.css']
})
export class Page1Component {
  modalRef?: BsModalRef;
  
  constructor(private modalService: BsModalService) {}
  
  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }
}
Component