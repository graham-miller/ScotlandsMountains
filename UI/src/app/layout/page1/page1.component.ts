import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { MountainsService } from 'src/app/services/mountains.service';

@Component({
  selector: 'app-page1',
  templateUrl: './page1.component.html',
  styleUrls: ['./page1.component.css']
})
export class Page1Component implements OnInit {
  modalRef?: BsModalRef;
  test: string = ''

  constructor(
    private modalService: BsModalService,
    private mountains: MountainsService) {}
  
  ngOnInit(): void {
    this.mountains.getClassifications().subscribe(x => {
      debugger;
      this.test = x
    });
  }
  
  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }
}
Component