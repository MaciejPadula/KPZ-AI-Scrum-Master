import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import { SessionTask } from '../../models/session-task';

@Component({
  selector: 'app-session-task',
  standalone: true,
  imports: [CommonModule, MaterialModule],
  templateUrl: './session-task.component.html',
})
export class SessionTaskComponent {
  task = input.required<SessionTask>();
}
