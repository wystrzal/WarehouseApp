import { Component, OnInit } from "@angular/core";
import { PlanService } from "../services/plan.service";
import { Plan } from "../models/Plan";
import { AuthService } from "../services/auth.service";
import { ErrorService } from "../services/error.service";

@Component({
  selector: "app-production",
  templateUrl: "./production.component.html",
  styleUrls: ["./production.component.css"]
})
export class ProductionComponent implements OnInit {
  lineName: string;
  plan: Plan[];

  constructor(
    private planService: PlanService,
    private authService: AuthService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {
    this.lineName = this.authService.decodedToken.unique_name;
    this.loadPlan();
    this.planService.refreshPlan.subscribe(() => {
      setTimeout(() => {
        this.loadPlan();
      }, 1000);
    });
  }

  refreshPlan() {
    this.loadPlan();
  }

  loadPlan() {
    this.planService.getProductionPlan(this.lineName).subscribe(
      (plan: Plan[]) => {
        this.plan = plan;
      },
      error => {
        this.errorService.newError(error);
      }
    );
  }

  deleteOrder(id: number) {
    this.errorService.deleteOrder(id, this.lineName);
  }
}
