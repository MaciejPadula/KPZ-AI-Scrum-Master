import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MenuComponent } from "./features/menu/components/menu.component";
import { MaterialModule } from './shared/material.module';
import { AsmLogoComponent } from './shared/components/asm-logo/asm-logo.component';
import { LoginComponent } from './shared/components/login/login.component';
import { TranslateModule } from '@ngx-translate/core';

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
        TranslateModule
    ]
})
export class AppComponent {
  title = 'artificial.scrum.master.frontend';
}
