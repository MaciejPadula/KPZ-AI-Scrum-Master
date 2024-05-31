import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '../../../../shared/material.module';
import { EstimationPokerDataService } from '../../services/estimation-poker-data.service';
import { EstimationPokerService } from '../../services/estimation-poker.service';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { finalize } from 'rxjs';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ToastService } from '../../../../shared/services/toast.service';
import { UserStorySelectorComponent } from '../user-story-selector/user-story-selector.component';
import { SelectedUserStory } from '../../models/selected-user-story';
import { AddTaskDialogService } from '../../services/add-task-dialog.service';
@Component({
  selector: 'app-add-task-dialog',
  standalone: true,
  templateUrl: './add-task-dialog.component.html',
  styleUrls: ['./add-task-dialog.component.scss'],
  imports: [
    CommonModule,
    MaterialModule,
    ReactiveFormsModule,
    TranslateModule,
    UserStorySelectorComponent,
  ],
})
export class AddTaskDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<AddTaskDialogComponent>);
  private readonly estimationPokerDataService = inject(
    EstimationPokerDataService
  );
  private readonly estimationPokerService = inject(EstimationPokerService);
  private readonly addTaskDialogService = inject(AddTaskDialogService);
  private readonly toastService = inject(ToastService);
  private readonly translateService = inject(TranslateService);
  private readonly sessionId: string = inject(MAT_DIALOG_DATA);

  #loading = signal<boolean>(false);
  public loading = computed(() => this.#loading() || this.addTaskDialogService.loader());

  public nameControl = new FormControl('', [
    Validators.required,
    Validators.minLength(3),
  ]);
  public descriptionControl = new FormControl('', [
    Validators.required,
    Validators.minLength(3),
  ]);
  public formGroup = new FormGroup({
    name: this.nameControl,
    description: this.descriptionControl,
  });

  public addTask() {
    this.#loading.set(true);
    this.estimationPokerDataService
      .addTask(
        this.sessionId,
        this.nameControl.value ?? '',
        this.descriptionControl.value ?? ''
      )
      .pipe(finalize(() => this.#loading.set(false)))
      .subscribe({
        next: () => {
          this.estimationPokerService.loadSessionTask(this.sessionId);
          this.dialogRef.close();
          this.toastService.openSuccess(
            this.translateService.instant('EstimationPoker.AddTask.Success')
          );
        },
        error: () => {
          this.toastService.openError(
            this.translateService.instant('EstimationPoker.AddTask.Error')
          );
        },
      });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  public storySelected(story: SelectedUserStory) {
    this.nameControl.setValue(story.name);
    this.descriptionControl.setValue(story.description);
  }
}
