import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Page1Component } from './page1/page1.component';
import { Page2Component } from './page2/page2.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AppBarComponent } from './app-bar/app-bar.component';

@NgModule({
  declarations: [
    Page1Component,
    Page2Component,
    PageNotFoundComponent,
    AppBarComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    Page1Component,
    Page2Component,
    PageNotFoundComponent,
    AppBarComponent
  ]
})
export class LayoutModule { }
