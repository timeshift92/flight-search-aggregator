﻿using System.Collections.Generic;
using System.Linq;
using ActualLab.CommandR;
using Aggregator.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Xunit;

namespace Aggregator.Tests.Features.Flights;

[TestSubject(typeof(TicketService))]
public class TicketServiceTest
{
    
    protected TestServer _testServer;

    [Fact]
    public async void CheckFilterForGetCitiesMethod()
    {
        await using var application = new WebApplicationFactory<Program>();
        var ticket = application.Services.GetRequiredService<ITicketService>();
        var comander = application.Services.GetRequiredService<ICommander>();
        var cities = new List<string>()
        {
            "Aaliyahberg","Aaliyahborough","Aaliyahburgh","Aaliyahchester","Aaliyahfort","Aaliyahfurt","Aaliyahhaven","Aaliyahland","Aaliyahmouth","Aaliyahport","Aaliyahshire","Aaliyahside","Aaliyahstad","Aaliyahton","Aaliyahtown","Aaliyahview","Aaliyahville","Aaronborough","Aaronburgh","Aaronbury","Aaronchester","Aaronfort","Aaronfurt","Aaronhaven","Aaronmouth","Aaronport","Aaronshire","Aaronside","Aaronstad","Aaronton","Aarontown","Aaronview","Abagailberg","Abagailborough","Abagailburgh","Abagailbury","Abagailfort","Abagailfurt","Abagailhaven","Abagailland","Abagailmouth","Abagailport","Abagailshire","Abagailside","Abagailton","Abagailtown","Abagailview","Abagailville","Abbeyberg","Abbeyborough","Abbeyburgh","Abbeybury","Abbeychester","Abbeyfort","Abbeyhaven","Abbeyland","Abbeymouth","Abbeyport","Abbeyshire","Abbeystad","Abbeytown","Abbeyview","Abbeyville","Abbieberg","Abbieborough","Abbiebury","Abbiechester","Abbiefort","Abbiehaven","Abbieland","Abbiemouth","Abbieport","Abbieshire","Abbieside","Abbiestad","Abbieton","Abbietown","Abbieview","Abbigailberg","Abbigailborough","Abbigailburgh","Abbigailbury","Abbigailchester","Abbigailfort","Abbigailfurt","Abbigailhaven","Abbigailland","Abbigailmouth","Abbigailshire","Abbigailside","Abbigailton","Abbigailtown","Abbigailview","Abbottberg","Abbottborough","Abbottburgh","Abbottbury","Abbottchester","Abbottfort","Abbottfurt","Abbotthaven","Abbottland","Abbottmouth","Abbottport","Abbottshire","Abbottside","Abbottstad","Abbottton","Abbotttown","Abbottview","Abbottville","Abbyberg","Abbyborough","Abbyburgh","Abbybury","Abbychester","Abbyfurt","Abbyhaven","Abbymouth","Abbyport","Abbyshire","Abbyside","Abbystad","Abbyton","Abbyview","Abbyville","Abdielberg","Abdielborough","Abdielburgh","Abdielbury","Abdielchester","Abdielfort","Abdielhaven","Abdielland","Abdielmouth","Abdielport","Abdielshire","Abdielside","Abdielview","Abdielville","Abdulberg","Abdulborough","Abdulburgh","Abdulbury","Abdulchester","Abdulfort","Abdulhaven","Abdullahberg","Abdullahborough","Abdullahburgh","Abdullahbury","Abdullahchester","Abdullahfort","Abdullahfurt","Abdullahland","Abdullahmouth","Abdullahport","Abdullahshire","Abdullahside","Abdullahstad","Abdullahton","Abdullahtown","Abdullahville","Abdulland","Abdulmouth","Abdulport","Abdulshire","Abdulside","Abdulstad","Abdulton","Abdultown","Abdulview","Abdulville","Abeberg","Abeborough","Abeburgh","Abebury","Abechester","Abefort","Abefurt","Abehaven","Abeland","Abelardoberg","Abelardoborough","Abelardoburgh","Abelardobury","Abelardochester","Abelardofort","Abelardofurt","Abelardohaven","Abelardoland","Abelardomouth","Abelardoport","Abelardoshire","Abelardoside","Abelardostad","Abelardoton","Abelardotown","Abelardoview","Abelardoville","Abelberg","Abelborough","Abelburgh","Abelbury","Abelfort","Abelfurt","Abelhaven","Abelmouth","Abelport","Abelshire","Abelstad","Abelton","Abeltown","Abelview","Abelville","Abemouth","Abeport","Abernathyberg","Abernathyborough","Abernathyburgh","Abernathybury","Abernathychester","Abernathyfort","Abernathyfurt","Abernathyhaven","Abernathyland","Abernathymouth","Abernathyport","Abernathyshire","Abernathyside","Abernathystad","Abernathyton","Abernathytown","Abernathyview","Abernathyville","Abeshire","Abeside","Abestad","Abetown","Abeview","Abeville","Abigailberg","Abigailborough","Abigailburgh","Abigailbury","Abigailchester","Abigailfort","Abigailfurt","Abigailmouth","Abigailport","Abigailshire","Abigailside","Abigailstad","Abigailton","Abigailtown","Abigailview","Abigailville","Abigaleberg","Abigaleborough","Abigalebury","Abigalefort","Abigalefurt","Abigalehaven","Abigaleland","Abigalemouth","Abigaleport","Abigaleshire","Abigaleside","Abigalestad","Abigaleton","Abigaletown","Abigaleview","Abigayleberg","Abigayleborough","Abigayleburgh","Abigaylebury","Abigaylechester","Abigaylefort","Abigaylefurt","Abigaylehaven","Abigayleland","Abigaylemouth","Abigayleport","Abigayleshire","Abigayleside","Abigaylestad","Abigayletown","Abigayleview","Abigayleville","Abnerborough","Abnerburgh","Abnerbury","Abnerchester","Abnerfurt","Abnerhaven","Abnerland","Abnermouth","Abnerport","Abnershire","Abnerside","Abnerstad","Abnerton","Abnertown","Abnerview","Abnerville","Abrahamberg","Abrahamborough","Abrahamburgh","Abrahambury","Abrahamchester","Abrahamfort","Abrahamfurt","Abrahamhaven","Abrahamland","Abrahammouth","Abrahamport","Abrahamshire","Abrahamside","Abrahamstad","Abrahamtown","Abrahamview","Abrahamville","Abshireberg","Abshireborough","Abshireburgh","Abshirebury","Abshirechester","Abshirefort","Abshirefurt","Abshirehaven","Abshireland","Abshiremouth","Abshireport","Abshireshire","Abshireside","Abshirestad","Abshireton","Abshiretown","Abshireview","Abshireville","Adaberg","Adaborough","Adaburgh","Adabury","Adachester","Adafurt","Adahberg","Adahborough","Adahburgh","Adahbury","Adahchester","Adahfort","Adahfurt","Adahhaven","Adahmouth","Adahport","Adahshire","Adahside","Adahstad","Adahton","Adahtown","Adahview","Adahville","Adalbertoberg","Adalbertoburgh","Adalbertochester","Adalbertofort","Adalbertofurt","Adalbertohaven","Adalbertoland","Adalbertomouth","Adalbertoport","Adalbertoshire","Adalbertoside","Adalbertostad","Adalbertoton","Adalbertotown","Adalbertoview","Adalineberg","Adalineborough","Adalineburgh","Adalinebury","Adalinechester","Adalinefort","Adalinefurt","Adalinehaven","Adalineland","Adalinemouth","Adalineport","Adalineshire","Adalineside","Adalinestad","Adalineton","Adalinetown","Adalineview","Adalineville","Adamborough","Adamburgh","Adambury","Adamchester","Adamfort","Adamhaven","Adamland","Adammouth","Adamouth","Adamport","Adamsberg","Adamsborough","Adamsburgh","Adamsbury","Adamschester","Adamsfort","Adamsfurt","Adamshaven","Adamshire","Adamside","Adamsland","Adamsmouth","Adamsport","Adamsshire","Adamsside","Adamsstad","Adamstad","Adamston","Adamstown","Adamsview","Adamsville","Adamton","Adamview","Adamville","Adanburgh","Adanbury","Adanchester","Adanfort","Adanfurt","Adanhaven","Adanland","Adanmouth","Adanport","Adanshire","Adanside","Adanstad","Adanton","Adantown","Adanview","Adanville","Adaport","Adashire","Adaside","Adastad","Adaton","Adatown","Adaview","Adaville","Addieberg","Addieborough","Addieburgh","Addiebury","Addiefort","Addiefurt","Addiehaven","Addieland","Addiemouth","Addieshire","Addieside","Addiestad","Addieton","Addieview","Addieville","Addisonberg","Addisonborough","Addisonburgh","Addisonbury","Addisonchester","Addisonfort","Addisonfurt","Addisonhaven","Addisonland","Addisonmouth","Addisonport","Addisonshire","Addisonside","Addisonstad","Addisonton","Addisontown","Addisonview","Addisonville","Adelaberg","Adelaborough","Adelabury","Adelachester","Adelafort","Adelafurt","Adelahaven","Adelaland","Adelamouth","Adelaport","Adelashire","Adelaside","Adelastad"
        };
        await comander.Call(new SetCitiesCommand(cities.ToHashSet(), CityTypeEnum.Arrival));
        var cts = await ticket.GetCities("Adelastad", CityTypeEnum.Arrival);
        Assert.True(cts.Contains("Adelastad") );
        Assert.Equal(cts.First(),"Adelastad" );

    }
}