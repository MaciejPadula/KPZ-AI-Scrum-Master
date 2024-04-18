import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatRippleModule } from '@angular/material/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatFormFieldModule } from '@angular/material/form-field';
import {
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
  MatDialogClose,
} from '@angular/material/dialog';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatButtonModule,
    MatInputModule,
    MatDividerModule,
    MatExpansionModule,
    MatIconModule,
    MatToolbarModule,
    MatProgressBarModule,
    MatMenuModule,
    MatSidenavModule,
    MatRippleModule,
    MatSlideToggleModule,
    MatFormFieldModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    MatProgressSpinner,
    MatCardModule,
  ],
  exports: [
    MatButtonModule,
    MatInputModule,
    MatDividerModule,
    MatExpansionModule,
    MatIconModule,
    MatToolbarModule,
    MatProgressBarModule,
    MatMenuModule,
    MatSidenavModule,
    MatRippleModule,
    MatSlideToggleModule,
    MatFormFieldModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    MatProgressSpinner,
    MatCardModule,
  ],
})
export class MaterialModule {}
