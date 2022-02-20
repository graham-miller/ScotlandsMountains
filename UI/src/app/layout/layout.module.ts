import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from '../layout/layout.component';
import { Page1Component } from './page1/page1.component';
import { Page2Component } from './page2/page2.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

@NgModule({
  declarations: [
    LayoutComponent,
    Page1Component,
    Page2Component,
    PageNotFoundComponent
  ],
  imports: [
    CommonModule,
  ],
  exports: [
    LayoutComponent,
    Page1Component,
    Page2Component,
    PageNotFoundComponent
  ]
})
export class LayoutModule { }
