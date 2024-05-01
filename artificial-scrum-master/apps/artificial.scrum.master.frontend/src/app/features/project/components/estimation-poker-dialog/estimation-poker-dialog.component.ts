import { Component, Inject, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PokerSessionsListComponent } from '../poker-sessions-list/poker-sessions-list.component';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-estimation-poker-dialog',
  standalone: true,
  templateUrl: './estimation-poker-dialog.component.html',
  imports: [
    CommonModule,
    PokerSessionsListComponent,
    MaterialModule,
    TranslateModule,
  ],
})
export class EstimationPokerDialogComponent {
  private readonly dialogRef = inject(
    MatDialogRef<EstimationPokerDialogComponent>
  );

  constructor(@Inject(MAT_DIALOG_DATA) public readonly projectId: number) {}

  public onNoClick(): void {
    this.dialogRef.close();
  }
}
