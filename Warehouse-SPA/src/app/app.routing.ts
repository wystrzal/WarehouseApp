import { Routes } from "@angular/router";
import { LoginComponent } from "./login/login.component";
import { ProductionComponent } from "./production/production.component";
import { WarehouseComponent } from "./warehouse/warehouse.component";
import { AuthGuard } from "./guards/auth.guard";

export const AppRoutes: Routes = [
  { path: "", component: LoginComponent },
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [AuthGuard],
    children: [
      {
        path: "warehouse",
        component: WarehouseComponent,
        data: { roles: "Warehouse" }
      },
      {
        path: "production",
        component: ProductionComponent,
        data: { roles: "Production" }
      }
    ]
  },
  { path: "**", redirectTo: "", pathMatch: "full" }
];
