import {
  ChangeDetectionStrategy,
  Component,
  inject,
  input,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { map, timer } from 'rxjs';
import { EstimationPokerService } from '../../services/estimation-poker.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EstimationPokerDataService } from '../../services/estimation-poker-data.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { MaterialModule } from '../../../../shared/material.module';
import { TranslateModule } from '@ngx-translate/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-developer-view',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule, ReactiveFormsModule],
  templateUrl: './developer-view.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DeveloperViewComponent {
  private readonly estimationPokerDataService = inject(EstimationPokerDataService);
  private readonly estimationPokerService = inject(EstimationPokerService);
  private readonly toastService = inject(ToastService);

  public sessionId = input.required<string>();
  public currentTask = this.estimationPokerService.sessionTask;
  public username = signal<string>('');
  public usernameControl = new FormControl<string>('');
  public estimationValues = signal<number[]>([1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144]);

  private readonly minUsernameLength = 3;

  constructor() {
    timer(0, 10000)
      .pipe(
        takeUntilDestroyed(),
        map(() => this.estimationPokerService.loadSessionTask(this.sessionId()))
      )
      .subscribe();

    this.usernameControl.valueChanges
      .pipe(takeUntilDestroyed())
      .subscribe((value) => {
        if (value && value?.length >= this.minUsernameLength) {
          this.username.set(value);
        } else {
          this.username.set('');
        }
      });
  }

  public estimateCurrentTask(value: number) {
    const task = this.currentTask();
    if (!task){
      this.toastService.openError('No task selected');
      return;
    }

    this.estimationPokerDataService.addTaskEstimation(
      task.id,
      this.username(),
      this.sessionId(),
      value
    ).subscribe({
      next: () => {
        this.estimationPokerService.loadSessionTask(this.sessionId());
      },
    });
  }
}
