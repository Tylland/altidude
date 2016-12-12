UPDATE UserView
SET ExperiencePoints = 10 + (SELECT COUNT(*) * 5 FROM ProfileEnvelope WHERE UserId = UserView.Id)
