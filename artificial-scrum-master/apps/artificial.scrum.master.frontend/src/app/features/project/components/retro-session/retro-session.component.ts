import {
  Component,
  OnInit,
  computed,
  inject,
  input,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RetroDataService } from '../../services/retro-data.service';
import { MaterialModule } from '../../../../shared/material.module';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-retro-session',
  standalone: true,
  imports: [CommonModule, MaterialModule, RouterModule, TranslateModule],
  templateUrl: './retro-session.component.html',
})
export class RetroSessionComponent implements OnInit {
  private readonly retroDataService = inject(RetroDataService);

  public sprintId = input.required<number>();
  public sprintName = input.required<string>();
  public projectId = input.required<number>();

  #sessionId = signal<string | null>(null);
  public sessionId = this.#sessionId.asReadonly();

  public ngOnInit(): void {
    this.retroDataService
      .createSessionIfNotExists(
        this.sprintId(),
        this.sprintName(),
        this.projectId()
      )
      .subscribe((response) => {
        this.#sessionId.set(response.sessionId);
      });
  }

  public linkToSession = computed(() => {
    return ['/Retrospective', this.#sessionId()];
  });
}
