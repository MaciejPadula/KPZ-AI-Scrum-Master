import { Injectable, computed, inject, signal } from '@angular/core';
import { EstimationPokerDataService } from './estimation-poker-data.service';
import { SessionTask } from '../models/session-task';
import { AuthorizationService } from '../../authorization/services/authorization-service';
import { TaskEstimation } from '../models/task-estimation';
import { Subject } from 'rxjs';
import { Session } from '../../../shared/models/session';
import { ToastService } from '../../../shared/services/toast.service';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class EstimationPokerService {
  private readonly dataService = inject(EstimationPokerDataService);
  private readonly authorizationService = inject(AuthorizationService);
  private readonly toastService = inject(ToastService);
  private readonly translateService = inject(TranslateService);

  public isSessionLoaded = computed(() => {
    return this.session() !== null;
  });
  public showScrumMasterView = computed(() => {
    return (
      this.authorizationService.isAuthorized() &&
      this.authorizationService.userId() === this.session()?.userId
    );
  });

  #session = signal<Session | null>(null);
  public session = this.#session.asReadonly();

  #taskEstimations = signal<TaskEstimation[]>([]);
  public taskEstimations = this.#taskEstimations.asReadonly();

  #averageTaskEstimation = signal<number>(0);
  public averageTaskEstimation = this.#averageTaskEstimation.asReadonly();

  #sessionTask = signal<SessionTask | null>(null);
  public sessionTask = this.#sessionTask.asReadonly();

  private readonly newTaskLoadedSubject$ = new Subject<void>();
  public newTaskLoaded$ = this.newTaskLoadedSubject$.asObservable();

  public loadSession(sessionId: string) {
    this.dataService.getSession(sessionId).subscribe({
      next: (session) => {
        this.#session.set(session);
      },
      error: () =>
        this.toastService.openError(
          this.translateService.instant('EstimationPoker.LoadSession.Error')
        ),
    });
  }

  public clearTaskEstimations() {
    this.#taskEstimations.set([]);
    this.#averageTaskEstimation.set(0);
  }

  public loadTaskEstimations(taskId: number) {
    this.dataService.getSessionTaskEstimations(taskId).subscribe({
      next: (response) => {
        this.#taskEstimations.set(response.estimations);
        this.#averageTaskEstimation.set(response.averageEstimation);
      },
      error: () =>
        this.toastService.openError(
          this.translateService.instant(
            'EstimationPoker.LoadTaskEstimations.Error'
          )
        ),
    });
  }

  public loadSessionTask(sessionId: string) {
    this.dataService.getSessionTask(sessionId).subscribe({
      next: (task) => {
        this.#sessionTask.update((oldTask) => {
          if (JSON.stringify(oldTask) !== JSON.stringify(task)) {
            this.newTaskLoadedSubject$.next();
          }
          return task;
        });
      },
      error: () =>
        this.toastService.openError(
          this.translateService.instant(
            'EstimationPoker.LoadTask.Error'
          )
        ),
    });
  }
}
