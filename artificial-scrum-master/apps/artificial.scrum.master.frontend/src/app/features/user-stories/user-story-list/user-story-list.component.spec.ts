import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserStoryListComponent } from './user-story-list.component';

describe('UserStoryListComponent', () => {
  let component: UserStoryListComponent;
  let fixture: ComponentFixture<UserStoryListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserStoryListComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(UserStoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
