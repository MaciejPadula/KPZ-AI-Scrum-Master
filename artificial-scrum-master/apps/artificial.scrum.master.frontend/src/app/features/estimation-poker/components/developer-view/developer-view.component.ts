import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  effect,
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
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { SessionTaskComponent } from "../session-task/session-task.component";
import { AuthorizationService } from '../../../authorization/services/authorization-service';
import { UsersListService } from '../../services/users-list.service';

@Component({
    selector: 'app-developer-view',
    standalone: true,
    templateUrl: './developer-view.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,
    imports: [CommonModule, MaterialModule, TranslateModule, ReactiveFormsModule, SessionTaskComponent]
})
export class DeveloperViewComponent implements OnInit {
  private readonly estimationPokerDataService = inject(EstimationPokerDataService);
  private readonly estimationPokerService = inject(EstimationPokerService);
  private readonly toastService = inject(ToastService);
  private readonly translationService = inject(TranslateService);
  private readonly userDataService = inject(AuthorizationService);
  private readonly usersListService = inject(UsersListService);

  #isRegistered = signal<boolean>(false);
  public isRegistered = this.#isRegistered.asReadonly();
  public sessionId = input.required<string>();
  public currentTask = this.estimationPokerService.sessionTask;
  public username = signal<string>('');
  public usernameControl = new FormControl<string>('');
  public estimationValues = signal<number[]>([1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144]);
  #disableEstiamtionButtons = signal<boolean>(false);
  public disableEstimationButtons = this.#disableEstiamtionButtons.asReadonly();
  public userData = this.userDataService.userData;

  private readonly minUsernameLength = 3;

  constructor() {
    effect(() => {
      const data = this.userData();
      if (data.isAuthorized) {
        this.usernameControl.setValue(data.userName);
      }
    });

    timer(0, 5000)
      .pipe(
        takeUntilDestroyed(),
        map(() => this.estimationPokerService.loadSessionTask(this.sessionId()))
      )
      .subscribe();

    this.estimationPokerService.newTaskLoaded$.pipe(takeUntilDestroyed()).subscribe(() => {
      this.#disableEstiamtionButtons.set(false);
    });

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

  public ngOnInit(): void {
    this.usersListService.connectDeveloper(this.sessionId());
  }

  public register() {
    if (!this.usernameControl.value || this.usernameControl.value.length < this.minUsernameLength) {
      this.toastService.openError(this.translationService.instant("EstimationPoker.UserNameTooShort"));
      return;
    }

    this.usersListService.registerUser(this.usernameControl.value);
    this.#isRegistered.set(true);
  }

  public estimateCurrentTask(value: number) {
    const task = this.currentTask();
    if (!task){
      this.toastService.openError(this.translationService.instant("EstimationPoker.NoTasks"));
      return;
    }

    if (!this.username()){
      this.toastService.openError(this.translationService.instant("EstimationPoker.UserNameNotSelected"));
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
        this.#disableEstiamtionButtons.set(true);
      },
    });
  }
}
