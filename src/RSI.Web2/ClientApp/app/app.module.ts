import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UniversalModule } from 'angular2-universal';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './components/app.component'
import { NavMenuComponent } from './components/navmenu.component';
import { HomeComponent } from './components/home.component';
import { PortfolioComponent } from './components/portfolio.component';
import { EditItemComponent } from './components/edit-item.component';
import { RankingComponent } from './components/ranking.component';
import { AllRankingComponent } from './components/allranking.component';
import { ChartsComponent } from './components/charts.component';
import { TradingService } from './services/trading.service';
import { DateService } from './services/date.service';

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        EditItemComponent,
        PortfolioComponent,
        RankingComponent,
        AllRankingComponent,
        ChartsComponent,
        HomeComponent
    ],
    providers: [ TradingService, DateService ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'portfolio', component: PortfolioComponent },
            { path: 'ranking', component: RankingComponent },
            { path: 'all', component: AllRankingComponent },
            { path: 'charts', component: ChartsComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModule {
}
