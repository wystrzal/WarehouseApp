import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { Plan } from "../models/Plan";
import { AuthService } from "./auth.service";
import { Subject } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class PlanService {
  refreshPlan = new Subject<MouseEvent>();
  baseUrl = environment.apiUrl + "plan/";

  constructor(private http: HttpClient, private authService: AuthService) {}

  getWarehousePlan(line: string) {
    return this.http.get<Plan[]>(this.baseUrl + "warehouse/" + line);
  }

  getProductionPlan(line: string) {
    return this.http.get<Plan[]>(this.baseUrl + "production/" + line);
  }

  updateStatus(id: number, model: any) {
    return this.http.put(this.baseUrl + id, model);
  }

  deleteOrder(id: number, line: string) {
    return this.http.post(this.baseUrl + line + "/delete/" + id, {});
  }

  addNewOrder(model: any, repeatAmount: number, line: string) {
    let params = new HttpParams();

    params = params.append("repeatAmount", repeatAmount.toString());

    return this.http.post(
      this.baseUrl + line + "/" + this.authService.decodedToken.nameid,
      model,
      { params }
    );
  }
}
