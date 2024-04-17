import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { TaskRow } from './task-row';

@Component({
  selector: 'app-task-row',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './task-row.component.html',
})
export class TaskRowComponent {
  @Input()
  public taskRow: TaskRow;
}
