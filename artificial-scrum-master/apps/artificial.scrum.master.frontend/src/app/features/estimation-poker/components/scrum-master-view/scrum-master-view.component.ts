import {
  ChangeDetectionStrategy,
  Component,
  inject,
  input,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { map, timer } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EstimationPokerService } from '../../services/estimation-poker.service';
import { EstimationPokerDataService } from '../../services/estimation-poker-data.service';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-scrum-master-view',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule],
  templateUrl: './scrum-master-view.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ScrumMasterViewComponent {
  private readonly estimationPokerDataService = inject(
    EstimationPokerDataService
  );
  private readonly estimationPokerService = inject(EstimationPokerService);

  public sessionId = input.required<string>();
  public currentTask = this.estimationPokerService.sessionTask;
  public estimations = this.estimationPokerService.taskEstimations;
  public averageEstimation = this.estimationPokerService.averageTaskEstimation;

  constructor() {
    timer(0, 10000)
      .pipe(
        takeUntilDestroyed(),
        map(() => this.estimationPokerService.loadSessionTask(this.sessionId()))
      )
      .subscribe();
  }

  public addTask() {
    this.estimationPokerDataService.addTask(
      this.sessionId(),
      'asfdsfsdf',
      'test'
    ).subscribe({
      next: () => {
        this.estimationPokerService.loadSessionTask(this.sessionId());
      },
    });
  }

  public revealEstimations() {
    const task = this.currentTask();
    if (task) {
      this.estimationPokerService.loadTaskEstimations(task.id);
    }
  }
}
