import { Component, inject, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { EstimationPokerDialogComponent } from '../estimation-poker-dialog/estimation-poker-dialog.component';
import { MaterialModule } from 'apps/artificial.scrum.master.frontend/src/app/shared/material.module';

@Component({
  selector: 'app-estimation-poker-button',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './estimation-poker-button.component.html',
})
export class EstimationPokerButtonComponent {
  private readonly dialog = inject(MatDialog);

  public projectId = input.required<number>();

  public openDialog() {
    this.dialog
      .open(EstimationPokerDialogComponent, {
        data: this.projectId(),
      })
      .afterClosed()
      .subscribe();
  }
}
