import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ProfilesComponent } from './components/profiles/profiles.component';
import { ProfileDetailsComponent } from './components/profile-details/profile-details.component';
import { NotFoundComponent } from './components/not-found/not-found.component';


const routes: Routes = [
  { path: 'dashboard', component: DashboardComponent },
  { path: 'profiles', component: ProfilesComponent },
  { path: 'profile/:id', component: ProfileDetailsComponent },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: '404', component: NotFoundComponent },
  { path: '**', redirectTo: '404' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
