import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../shared/material.module';
import { LoginComponent } from "../../../shared/components/login/login.component";
import { AsmLogoComponent } from "../../../shared/components/asm-logo/asm-logo.component";
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-menu',
    standalone: true,
    templateUrl: './menu.component.html',
    imports: [CommonModule, MaterialModule, LoginComponent, AsmLogoComponent, RouterLink]
})
export class MenuComponent {}
