import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ImageLoaderDirective } from '../../directives/image-loader-directive';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-asm-logo',
  standalone: true,
  imports: [CommonModule, ImageLoaderDirective, RouterLink],
  templateUrl: './asm-logo.component.html'
})
export class AsmLogoComponent {}
