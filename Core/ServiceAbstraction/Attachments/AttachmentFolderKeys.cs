namespace ServiceAbstraction.Attachments
{
    /// <summary>
    /// Logical names for the folders attachments are grouped into. The physical location of each
    /// one is configuration (<c>Attachments:Folders</c>), so the server owns the storage layout and
    /// the client never gets to choose where a file lands.
    /// </summary>
    public static class AttachmentFolderKeys
    {
        public const string ProfileImage = nameof(ProfileImage);
        public const string AboutImage = nameof(AboutImage);
        public const string ProjectImage = nameof(ProjectImage);
        public const string ProjectGalleryImage = nameof(ProjectGalleryImage);
        public const string AchievementImage = nameof(AchievementImage);
        public const string BlogPostCoverImage = nameof(BlogPostCoverImage);
        public const string ServiceIcon = nameof(ServiceIcon);
        public const string ExperienceLogo = nameof(ExperienceLogo);
        public const string EducationLogo = nameof(EducationLogo);
        public const string TechnologyIcon = nameof(TechnologyIcon);
        public const string SkillIcon = nameof(SkillIcon);
        public const string SocialLinkIcon = nameof(SocialLinkIcon);
        public const string ResumeFile = nameof(ResumeFile);
        public const string CertificateFile = nameof(CertificateFile);
    }
}
