import { PageNotFoundComponent } from './layout/page-not-found/page-not-found.component';
import { Page1Component } from './layout/page1/page1.component';
import { Page2Component } from './layout/page2/page2.component';

export const Routes = [
    {path: 'page-1', component: Page1Component},
    {path: 'page-2', component: Page2Component},
    {path: '', redirectTo: '/page-1', pathMatch: 'full'},
    {path: '**', component: PageNotFoundComponent}
  ]