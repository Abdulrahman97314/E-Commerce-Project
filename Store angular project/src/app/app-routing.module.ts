import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductsComponent } from './components/products/products.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { DetailsComponent } from './components/details/details.component';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { AuthGuard } from './Guards/auth.guard';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { ForgetPasswordComponent } from './components/forget-password/forget-password.component';
import { BasketComponent } from './components/basket/basket.component';
import { OrderComponent } from './components/order/order.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { MyOrdersComponent } from './components/my-orders/my-orders.component';

const routes: Routes = [
  {path:"",redirectTo:"products",pathMatch:"full"},
  {path:"products",component:ProductsComponent},
  {path: 'products/:id', component: DetailsComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'forgetPassword', component: ForgetPasswordComponent},
  {path: 'resetPassword', component: ResetPasswordComponent},
  {path: 'basket', component: BasketComponent},
  {path: 'changePassword',canActivate:[AuthGuard],component: ChangePasswordComponent},
  {path: 'myOrders',canActivate:[AuthGuard],component: MyOrdersComponent},
  {path: 'order',canActivate:[AuthGuard], component: OrderComponent},
  {path: 'login', component: LoginComponent},
  {path:"**",component:NotFoundComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
