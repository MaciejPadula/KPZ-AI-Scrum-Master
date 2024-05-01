import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../shared/material.module';
import { AuthorizationService } from '../authorization/services/authorization-service';
import { ImageLoaderDirective } from '../../shared/directives/image-loader-directive';
import { GoogleSigninButtonModule } from '@abacritt/angularx-social-login';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-welcome',
  standalone: true,
  imports: [CommonModule, MaterialModule, ImageLoaderDirective, GoogleSigninButtonModule, TranslateModule],
  templateUrl: './welcome.component.html',
  styleUrl: './welcome.component.css',
})
export class WelcomeComponent {
  private authService = inject(AuthorizationService);
  isLoggedIn = this.authService.isAuthorized;
}
