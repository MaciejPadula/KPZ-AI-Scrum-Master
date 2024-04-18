import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { TaskRow } from './task-row';
import { AvatarComponent } from '../avatar/avatar.component';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-task-row',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatChipsModule, AvatarComponent],
  templateUrl: './task-row.component.html',
})
export class TaskRowComponent {
  @Input()
  public taskRow: TaskRow;
}
