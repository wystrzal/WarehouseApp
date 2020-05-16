import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import { map } from "rxjs/operators";
import { JwtHelperService } from "@auth0/angular-jwt";
import { Router } from "@angular/router";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  baseUrl = environment.apiUrl + "auth/";
  jwtHelper = new JwtHelperService();
  decodedToken: any;

  constructor(private http: HttpClient, private route: Router) {}

  login(model: any) {
    return this.http.post(this.baseUrl + "login", model).pipe(
      map((response: any) => {
        if (response) {
          localStorage.setItem("token", response.token);
          this.decodedToken = this.jwtHelper.decodeToken(response.token);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem("token");
    this.decodedToken = null;
    this.route.navigate([""]);
  }

  loggedIn() {
    const token = localStorage.getItem("token");
    return !this.jwtHelper.isTokenExpired(token);
  }

  roleMatch(allowedRoles: string): boolean {
    const userRoles = this.decodedToken.role.toString();
    if (allowedRoles === userRoles) {
      return true;
    }
    return false;
  }
}
