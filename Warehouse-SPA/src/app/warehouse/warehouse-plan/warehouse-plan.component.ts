import { Component, OnInit, Input, OnChanges } from "@angular/core";
import { Plan } from "src/app/models/Plan";
import { PlanService } from "src/app/services/plan.service";
import { map } from "rxjs/operators";
import { ErrorService } from "src/app/services/error.service";

@Component({
  selector: "app-warehouse-plan",
  templateUrl: "./warehouse-plan.component.html",
  styleUrls: ["./warehouse-plan.component.css"]
})
export class WarehousePlanComponent implements OnInit, OnChanges {
  @Input() lineName: string;
  plan: Plan[];
  styleCover: any[][] = [["bc1"], ["bc2"], ["bc3"], ["bc4"]];

  constructor(
    private planService: PlanService,
    private errorService: ErrorService
  ) {}

  ngOnInit() {
    this.loadPlan();
    console.log(this.styleCover);
  }

  ngOnChanges() {
    this.loadPlan();
  }

  loadPlan() {
    this.planService
      .getWarehousePlan(this.lineName)
      .subscribe((plan: Plan[]) => {
        this.plan = plan;
      });
  }

  changeStatus(id: number, index: number) {
    if (this.plan[index].status === "Nowe") {
      this.plan[index].status = "Załadowane";
    } else {
      this.plan[index].status = "Dostarczone";
    }
    this.planService.updateStatus(id, this.plan[index]).subscribe(
      () => {},
      error => {
        this.errorService.newError(error);
      }
    );
  }

  resetStatus(id: number, index: number) {
    this.plan[index].status = "Nowe";
    this.planService.updateStatus(id, this.plan[index]).subscribe(
      () => {},
      error => {
        this.errorService.newError(error);
      }
    );
  }
}
