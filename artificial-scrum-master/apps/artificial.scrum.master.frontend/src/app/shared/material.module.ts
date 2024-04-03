import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatButtonModule,
    MatInputModule,
    MatDividerModule,
    MatExpansionModule,
    MatIconModule,
    MatProgressBarModule,
  ],
  exports: [
    MatButtonModule,
    MatInputModule,
    MatDividerModule,
    MatExpansionModule,
    MatIconModule,
    MatProgressBarModule,
  ],
})
export class MaterialModule {}
