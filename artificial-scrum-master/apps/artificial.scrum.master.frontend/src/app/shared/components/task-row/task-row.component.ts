import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskRow } from './task-row';
import { AvatarComponent } from '../avatar/avatar.component';
import { MaterialModule } from '../../material.module';

@Component({
  selector: 'app-task-row',
  standalone: true,
  imports: [CommonModule, AvatarComponent, MaterialModule],
  templateUrl: './task-row.component.html',
})
export class TaskRowComponent {
  @Input()
  public taskRow: TaskRow;
}
