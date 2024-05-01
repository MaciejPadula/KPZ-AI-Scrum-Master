import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MenuComponent } from "./features/menu/components/menu.component";
import { MaterialModule } from './shared/material.module';
import { AsmLogoComponent } from './shared/components/asm-logo/asm-logo.component';
import { LoginComponent } from './shared/components/login/login.component';
import { TranslateModule } from '@ngx-translate/core';
import { ThemeSwitchComponent } from "./features/menu/components/theme-switch.component";
import { AuthorizationService } from './features/authorization/services/authorization-service';

@Component({
    standalone: true,
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss',
    imports: [
        RouterModule,
        MenuComponent,
        MaterialModule,
        AsmLogoComponent,
        LoginComponent,
        TranslateModule,
        ThemeSwitchComponent
    ]
})
export class AppComponent {
  title = 'Artificial Scrum Master';

  private authService = inject(AuthorizationService);
  isAuthenticated = this.authService.isAuthorized;
}
