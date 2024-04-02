import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeedListComponent } from "./components/feed-list/feed-list.component";

@Component({
    selector: 'app-feed',
    standalone: true,
    templateUrl: './feed.component.html',
    imports: [CommonModule, FeedListComponent],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FeedComponent {}
