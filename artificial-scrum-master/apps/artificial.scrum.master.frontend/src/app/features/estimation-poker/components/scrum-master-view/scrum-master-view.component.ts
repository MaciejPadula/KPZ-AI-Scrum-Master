import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  input,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { map, timer } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EstimationPokerService } from '../../services/estimation-poker.service';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { MatDialog } from '@angular/material/dialog';
import { AddTaskDialogComponent } from '../add-task-dialog/add-task-dialog.component';
import { SessionTaskComponent } from '../session-task/session-task.component';

@Component({
  selector: 'app-scrum-master-view',
  standalone: true,
  templateUrl: './scrum-master-view.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    MaterialModule,
    TranslateModule,
    SessionTaskComponent,
  ],
})
export class ScrumMasterViewComponent {
  private readonly estimationPokerService = inject(EstimationPokerService);
  private readonly dialog = inject(MatDialog);

  public sessionId = input.required<string>();
  public currentTask = this.estimationPokerService.sessionTask;
  public estimations = this.estimationPokerService.taskEstimations;
  public averageEstimation = computed(() => this.estimationPokerService.averageTaskEstimation().toFixed(2));

  constructor() {
    timer(0, 5000)
      .pipe(
        takeUntilDestroyed(),
        map(() => this.estimationPokerService.loadSessionTask(this.sessionId()))
      )
      .subscribe();
  }

  public addTask() {
    this.dialog
      .open(AddTaskDialogComponent, {
        data: this.sessionId(),
      })
      .afterClosed()
      .subscribe(() => {
        this.estimationPokerService.clearTaskEstimations();
      });
  }

  public revealEstimations() {
    const task = this.currentTask();
    if (task) {
      this.estimationPokerService.loadTaskEstimations(task.id);
    }
  }
}
