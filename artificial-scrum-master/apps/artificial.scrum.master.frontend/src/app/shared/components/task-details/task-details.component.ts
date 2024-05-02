import {
  Component,
  computed,
  inject,
  Inject,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TaskDetails } from './task-details';
import { HttpClient } from '@angular/common/http';
import { MaterialModule } from '../../material.module';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-task-details',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule],
  templateUrl: './task-details.component.html',
  styleUrls: ['./task-details.component.scss'],
})
export class TaskDetailsComponent implements OnInit {
  details = signal<TaskDetails | null>(null);
  error = signal<boolean>(false);
  formattedDate = computed(() => this.formatDate(this.details()?.createdDate));

  #httpClient = inject(HttpClient);
  #taskId: number;

  constructor(@Inject(MAT_DIALOG_DATA) taskId: number) {
    this.#taskId = taskId;
  }

  ngOnInit(): void {
    this.#httpClient.get<TaskDetails>(`/api/task/${this.#taskId}`).subscribe({
      next: (response) => this.details.set(response),
      error: () => this.error.set(true),
    });
  }

  private formatDate(dateInput: Date | undefined): string {
    if (!dateInput) {
      return '';
    }
    const date = new Date(dateInput);
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear();
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    return `${day}.${month}.${year} ${hours}:${minutes}`;
  }
}
