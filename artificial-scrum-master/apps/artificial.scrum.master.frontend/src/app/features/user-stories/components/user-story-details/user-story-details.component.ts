import { Component, Inject, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from 'apps/artificial.scrum.master.frontend/src/app/shared/material.module';
import { UserStoryDetails } from '../../models/user-story-details';
import { HttpClient } from '@angular/common/http';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-user-story-details',
  standalone: true,
  imports: [CommonModule, MaterialModule, TranslateModule],
  templateUrl: './user-story-details.component.html',
  styleUrls: ['./user-story-details.component.scss']
})
export class UserStoryDetailsComponent implements OnInit {

  details = signal<UserStoryDetails | null>(null);
  error = signal<boolean>(false);
  #httpClient = inject(HttpClient);
  #storyId: number;

  constructor(@Inject(MAT_DIALOG_DATA) storyId: number) {
    this.#storyId = storyId;
  }

  ngOnInit(): void {
    this.#httpClient.get<UserStoryDetails>(`/api/userStories/${this.#storyId}`).subscribe(
      {
        next:
          response => this.details.set(response),
        error: () => this.error.set(true)
      }
    );
  }
}
