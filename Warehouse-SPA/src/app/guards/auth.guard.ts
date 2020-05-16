import { Injectable } from "@angular/core";
import { CanActivate, Router, ActivatedRouteSnapshot } from "@angular/router";
import { AuthService } from "../services/auth.service";
import { ErrorService } from "../services/error.service";

@Injectable({
  providedIn: "root"
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private errorService: ErrorService
  ) {}

  canActivate(next: ActivatedRouteSnapshot): boolean {
    const role = next.firstChild.data.roles as string;
    if (role) {
      const match = this.authService.roleMatch(role);
      if (match) {
        return true;
      } else {
        this.router.navigate([""]);
        this.errorService.newError("Brak autoryzacji.");
      }
    }
    if (this.authService.loggedIn()) {
      return true;
    }
    this.errorService.newError("Brak autoryzacji.");

    this.router.navigate([""]);
    return false;
  }
}
