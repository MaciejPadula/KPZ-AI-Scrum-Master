import { Component, Inject, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../../../shared/material.module';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectPokerDataService } from '../../services/project-poker-data.service';
import { ToastService } from '../../../../shared/services/toast.service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-add-poker-session-dialog',
  standalone: true,
  imports: [CommonModule, MaterialModule, ReactiveFormsModule, TranslateModule],
  templateUrl: './add-poker-session-dialog.component.html',
})
export class AddPokerSessionDialogComponent {
  private readonly dialogRef = inject(
    MatDialogRef<AddPokerSessionDialogComponent>
  );
  private readonly pokerDataService = inject(ProjectPokerDataService);
  private readonly toastService = inject(ToastService);
  private readonly translateService = inject(TranslateService);

  public nameControl = new FormControl('', [
    Validators.required,
    Validators.minLength(3),
  ]);

  public formGroup = new FormGroup({
    name: this.nameControl,
  });

  #loading = signal<boolean>(false);
  public loading = this.#loading.asReadonly();

  constructor(@Inject(MAT_DIALOG_DATA) private readonly projectId: number) {}

  public createSession() {
    this.#loading.set(true);
    this.pokerDataService
      .createSession(this.nameControl.value ?? '', this.projectId)
      .pipe(finalize(() => this.#loading.set(false)))
      .subscribe({
        next: () => {
          this.toastService.openSuccess(
            this.translateService.instant('Project.Poker.AddSession.Success')
          );
          this.dialogRef.close();
        },
        error: () =>
          this.toastService.openError(
            this.translateService.instant('Project.Poker.AddSession.Error')
          ),
      });
  }

  public onNoClick(): void {
    this.dialogRef.close();
  }
}
