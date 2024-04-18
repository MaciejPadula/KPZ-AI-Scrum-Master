import { Component, Inject, inject, signal } from '@angular/core';
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
import { TranslateModule } from '@ngx-translate/core';
@Component({
  selector: 'app-add-task-dialog',
  standalone: true,
  imports: [CommonModule, MaterialModule, ReactiveFormsModule, TranslateModule],
  templateUrl: './add-task-dialog.component.html',
})
export class AddTaskDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<AddTaskDialogComponent>);
  private readonly estimationPokerDataService = inject(
    EstimationPokerDataService
  );
  private readonly estimationPokerService = inject(EstimationPokerService);

  #loading = signal<boolean>(false);
  public loading = this.#loading.asReadonly();

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

  constructor(@Inject(MAT_DIALOG_DATA) private readonly sessionId: string) {}

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
        },
      });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
