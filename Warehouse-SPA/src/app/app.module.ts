import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { AppRoutes } from "./app.routing";
import { FormsModule } from "@angular/forms";
import { JwtModule } from "@auth0/angular-jwt";
import { ErrorInterceptorProvider } from "./services/error.interceptor";
import { TabsModule } from "ngx-bootstrap/tabs";

import { AppComponent } from "./app.component";
import { LoginComponent } from "./login/login.component";
import { ProductionComponent } from "./production/production.component";
import { HttpClientModule } from "@angular/common/http";
import { WarehouseComponent } from "./warehouse/warehouse.component";
import { CommonModule } from "@angular/common";
import { NavComponent } from "./nav/nav.component";
import { WarehousePlanComponent } from "./warehouse/warehouse-plan/warehouse-plan.component";
import { ModalModule } from "ngx-bootstrap/modal";
import { AddModalComponent } from "./production/add-modal/add-modal.component";
import { CustomMaxDirective } from "./directives/custom-max-validator.directive";
import { ErrorModalComponent } from "./helpers/error-modal/error-modal.component";

export function tokenGetter() {
  return localStorage.getItem("token");
}

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ProductionComponent,
    WarehouseComponent,
    WarehousePlanComponent,
    AddModalComponent,
    ErrorModalComponent,
    NavComponent,
    CustomMaxDirective
  ],
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(AppRoutes),
    FormsModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ["localhost:5000"],
        blacklistedRoutes: ["localhost:5000/api/auth"]
      }
    }),
    TabsModule.forRoot(),
    ModalModule.forRoot()
  ],
  providers: [ErrorInterceptorProvider],
  bootstrap: [AppComponent],
  entryComponents: [AddModalComponent, ErrorModalComponent]
})
export class AppModule {}
