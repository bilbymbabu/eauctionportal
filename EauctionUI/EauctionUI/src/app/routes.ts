import { Routes } from '@angular/router'
import { HomeComponent } from './home/home.component';
import { NavbarComponent } from './navbar/navbar.component';
import { ViewproductsComponent } from './viewproducts/viewproducts.component';

export const appRoutes: Routes = [
    { path: 'Home', component: HomeComponent },
    { path : '', redirectTo:'/ViewProducts', pathMatch : 'full'},
    {path:'ViewProducts',component:ViewproductsComponent}
];