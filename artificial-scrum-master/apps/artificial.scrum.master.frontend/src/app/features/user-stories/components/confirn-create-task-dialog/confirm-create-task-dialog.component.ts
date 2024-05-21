import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MaterialModule } from 'apps/artificial.scrum.master.frontend/src/app/shared/material.module';

@Component({
  selector: 'app-confirm-create-task-dialog',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './confirm-create-task-dialog.component.html',
})
export class ConfirmCreateTaskDialogComponent {

  data = inject(MAT_DIALOG_DATA);
}
