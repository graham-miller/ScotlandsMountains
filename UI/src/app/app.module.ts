import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { ClassificationsComponent } from './components/classifications/classifications.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { LoadingComponent } from './components/loading/loading.component';
import { MetersToFeetPipe } from './pipes/meters-to-feet.pipe';
import { FormsModule } from '@angular/forms';
import { SortableHeader } from './directives/sortable-header';
import { MountainTableComponent } from './components/mountain-table/mountain-table.component';
import { MountainComponent } from './components/mountain/mountain.component';
import { SentencePipe } from './pipes/sentence.pipe';
import { LogoComponent } from './components/logo/logo.component';
import { MapComponent } from './components/map/map.component';

@NgModule({
  declarations: [
    AppComponent,
    ClassificationsComponent,
    PageNotFoundComponent,
    NavBarComponent,
    LoadingComponent,
    MetersToFeetPipe,
    SortableHeader,
    MountainTableComponent,
    MountainComponent,
    SentencePipe,
    LogoComponent,
    MapComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgbModule
  ],
  providers: [
    MetersToFeetPipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
