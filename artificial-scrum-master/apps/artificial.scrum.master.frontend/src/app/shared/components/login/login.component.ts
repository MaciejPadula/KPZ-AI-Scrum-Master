import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GoogleSigninButtonModule } from "@abacritt/angularx-social-login";
import { AuthorizationService } from '../../../features/authorization/services/authorization-service';
import { MaterialModule } from '../../material.module';
import { ImageLoaderDirective } from '../../directives/image-loader-directive';
import { RouterLink } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, GoogleSigninButtonModule, MaterialModule, ImageLoaderDirective, RouterLink, TranslateModule],
  templateUrl: 'login.component.html',
})
export class LoginComponent {

  private authService = inject(AuthorizationService);

  userData = this.authService.userData;

  logout() {
    this.authService.logout();
  }
}
