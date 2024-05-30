import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-sprint-stories-priority',
  standalone: true,
  imports: [CommonModule, MatDialogModule, TranslateModule],
  templateUrl: './sprint-stories-priority.component.html',
  styleUrl: './sprint-stories-priority.component.css',
})
export class SprintStoriesPriorityComponent {
  #sprintId: number;
  public sprintId: number;

  #projectId: number;
  public projectId: number;

  constructor(
    @Inject(MAT_DIALOG_DATA) data: { sprintId: number; projectId: number }
  ) {
    this.#sprintId = data.sprintId;
    this.sprintId = data.sprintId;
    this.#projectId = data.projectId;
    this.projectId = data.projectId;
  }
}
