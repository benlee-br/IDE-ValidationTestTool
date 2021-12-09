using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Mail;
using System.Collections.Generic;

using BioRad.Common.DiagnosticsLogger;

namespace BioRad.Common
{
	#region Documentation Tags
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	/// <classinformation>
	///		<list type="bullet">
	///			<item name="authors">Authors:</item>
	///			<item name="review">Last design/code review:</item>
	///			<item name="requirementid">Requirement ID # : 
	///				<see href=""></see> 
	///			</item>
	///			<item name="classdiagram">
	///				<see href="Reference\FileORImageName">Class Diagram</see> 
	///			</item>
	///		</list>
	/// </classinformation>
	/// <archiveinformation>
	///		<list type="bullet">
	///			<item name="vssfile">$Workfile: Notifications.cs $</item>
	///			<item name="vssfilepath">$Archive: /PCRDev/Source/Core/Common/Common/Notifications.cs $</item>
	///			<item name="vssrevision">$Revision: 27 $</item>
	///			<item name="vssauthor">Last Check-in by:$Author: Vnguyen $</item>
	///			<item name="vssdate">$Date: 10/05/10 1:32p $</item>
	///		</list>
	/// </archiveinformation>
	#endregion

	public partial class Notifications
	{
		#region Constructors and Destructor

		/// <summary>Initializes a new instance of the Notifications class.</summary>
		public Notifications() { }

		#endregion

		#region Methods
		/// <summary>
		/// Sends run completed email.
		/// </summary>
		/// <param name="sendTo"></param>
		/// <param name="sendCC"></param>
		/// <param name="subjectLine"></param>
		/// <param name="attachments"></param>
		/// <param name="blockName"></param>
		/// <param name="status"></param>
		/// <param name="isSimulated"></param>
		/// <returns>True if success</returns>
		public static bool SendRunCompletedEmail(string sendTo, string sendCC, string subjectLine, string[] attachments,
			 string blockName, string status, bool isSimulated)
		{
			// 10/04/2010 VN - Defect 12109 - Concatinate subject line
			string subject = string.Format(Properties.Resources.Email_RunCompleteSubject, blockName);
			if(!string.IsNullOrEmpty(subjectLine))
				subject = string.Format("{0} - {1}", subjectLine, subject);

			string simulationText = Properties.Resources.Simulated + " ";
			if (!isSimulated)
			{
				simulationText = "";
			}
			string message = string.Format(Properties.Resources.Email_RunCompleteMessage_1,
				 simulationText, blockName, DateTime.Now.ToString(), status);

			// Add run 1 of 20 to body of message
			if (!string.IsNullOrEmpty(subjectLine))
			{
				message = string.Format("{0}\n\n{1}", message, subjectLine);
			}

			//Send the mail
			return SendEmail(sendTo, sendCC, null, subject, message, attachments, false);
		}
		/// <summary>
		/// Generic\basic mail delivery.
		/// </summary>
		/// <param name="sendTo"></param>
		/// <param name="sendCC"></param>
		/// <param name="sendBCC"></param>
		/// <param name="subject"></param>
		/// <param name="message"></param>
		/// <param name="attachments"></param>
		/// <param name="isBodyHtml"></param>
		/// <returns>True if success</returns>
		public static bool SendEmail(string sendTo,
			 string sendCC, string sendBCC, string subject,
			 string message, string[] attachments, bool isBodyHtml)
		{
			//logger..
			DiagnosticsLog log = DiagnosticsLogService.GetService.GetDiagnosticsLog
							(WellKnownLogName.Notifications);
			try
			{
				string logMessage = message;

				if (
					string.IsNullOrEmpty(sendTo) &&
					string.IsNullOrEmpty(sendCC) &&
					string.IsNullOrEmpty(sendBCC))
				{
					return false;
				}

				EmailOptions emailOptions = PersistedApplicationOptions.GetInstance.EmailOptions;

				string from = ApplicationStateData.GetInstance.ProductName;

				//attach std footer
				if (attachments != null && attachments.Length > 0)
				{
					// Fix for Bug 9319
					// attachments always has 2 objects - need to check each of them to see if it is null
					bool attachmentFound = false;
					foreach (string attachment in attachments)
					{
						if (!string.IsNullOrEmpty(attachment))
						{
							attachmentFound = true;
							break;
						}
					}

					if(attachmentFound)
						message = string.Concat(message,
							 "\n\n",
							Properties.Resources.Email_ResultsMessage);
				}
				message = string.Concat(message,
					 "\n\n\n",
					 "****************************************************",
					 "\n",
					 Properties.Resources.Email_DoNotReplyFooter,
					 "\n",
					 string.Format(Properties.Resources.Email_DoNotReceiveMessage,
					 ApplicationStateData.GetInstance.ProductName));

				using (MailMessage mail = CreateNewMailMessage(emailOptions))
				{
					// Defect 8777: validate each email address list.
					if (sendTo != null && sendTo != string.Empty)
					{
						sendTo = MakeEmailAddressesCommaDelimited(sendTo);
						mail.To.Add(sendTo);
					}
					if (sendCC != null && sendCC != string.Empty)
					{
						sendCC = MakeEmailAddressesCommaDelimited(sendCC);
						mail.CC.Add(sendCC);
					}
					if (sendBCC != null && sendBCC != string.Empty)
					{
						sendBCC = MakeEmailAddressesCommaDelimited(sendBCC);
						mail.Bcc.Add(sendBCC);
					}

					mail.Subject = subject;
					mail.IsBodyHtml = isBodyHtml;
					mail.Body = message;

					if (attachments != null)
					{
						foreach (string attachment in attachments)
						{
							if (attachment != null && attachment != "")
							{
								if (File.Exists(attachment))
								{
									mail.Attachments.Add(new Attachment(attachment));
								}
							}
						}
					}
					//Create smtp client, host is set from App.Config file already.
					SmtpClient smtpClient = CreateNewSmtpClient(emailOptions);
					smtpClient.Send(mail);
				}

				log.Info(string.Concat("eMail sent to : ", sendTo, ",", sendCC, ",", sendBCC, ",", logMessage));

				return true;
			}
            catch (Exception ex) //Logged
			{
				// Fix for Bug 10766
				log.SeriousError(string.Concat(ex.Message, " InnerEx:", ex.InnerException),
					 DiagnosticTag.EXCEPTION, ex);

				return false;
			}
		}
		/// <summary>
		/// Creates new Smtp client based on the provided EmailOptions.
		/// </summary>
		/// <param name="eo">email options</param>
		/// <returns>new smtp client</returns>
		public static SmtpClient CreateNewSmtpClient(EmailOptions eo)
		{
			SmtpClient smtpClient = new SmtpClient(eo.SmtpServerName, eo.Port);
			smtpClient.EnableSsl = eo.UseSsl;
			if (eo.AuthenticationRequired)
				smtpClient.Credentials = new NetworkCredential(eo.UserName, eo.Password);
			smtpClient.Timeout = (int)(eo.Timeout * 60 * 1000);
			return smtpClient;
		}

		/// <summary>
		/// Creates a new mail message using the EmailOptions provided to fill in From address and 
		/// from alias.
		/// </summary>
		/// <param name="eo">email options</param>
		/// <returns>new, initialized MailMessage</returns>
		public static MailMessage CreateNewMailMessage(EmailOptions eo)
		{
			MailMessage mail = new MailMessage();

			//Add alias to from address that is set in App.Config file already.
			//get local machine's full address
			string machineDomain = Properties.Resources.UnknownComputer;
			try
			{
				IPHostEntry entry = Dns.GetHostEntry(Environment.MachineName);
				machineDomain = entry.HostName;
			}
			catch
			{
				//Do nothing.
			}
			string fromAlias = string.Format(Properties.Resources.ProductNameOnComputerName,
					ApplicationStateData.GetInstance.ProductName,
					machineDomain);
			string fromAddress;
			if (eo.UseDefaultFromAddress)
				fromAddress = "DoNotReply@Bio-Rad.com";
			else
				fromAddress = eo.FromAddress;
			mail.From = new MailAddress(fromAddress, fromAlias);

			return mail;
		}

		/// <summary>
		/// replaces carriage returns with commas, removes trailing/leading commas and internal multi-commas.
		/// </summary>
		/// <param name="addresses">email addresses</param>
		/// <returns>validated email addresses.</returns>
		public static string MakeEmailAddressesCommaDelimited(string addresses)
		{
			// Replace carriage returns with commas.
			addresses = addresses.Replace("\r\n", ",");

			// Remove multi-commas
			int previousLength;
			do
			{
				previousLength = addresses.Length;
				addresses = addresses.Replace(",,", ",");
			} while (addresses.Length < previousLength);

			// Remove any address which is all whitespace.
			string[] addressArray = addresses.Split(',');
			StringBuilder sb = new StringBuilder();
			foreach (string address in addressArray)
			{
				if (string.IsNullOrEmpty(address.Trim()) == false)
					sb.Append(address).Append(",");
			}

			// Remove trailing comma.
			string ret = sb.ToString().Trim(',');

			return ret;
		}

		/// <summary>
		/// Send email aftr run is completed
		/// </summary>
		/// <param name="emailInfo">The EmailInfo object</param>
		/// <returns></returns>
		public static bool SendEmail(EmailInfo emailInfo)
		{
			if (emailInfo == null)
				return false;

			if (emailInfo.Attachments == null)
				emailInfo.Attachments = new List<string>();

			return SendRunCompletedEmail(
				emailInfo.SendTo,
				emailInfo.SendCC,
				emailInfo.Subject,
				emailInfo.Attachments.ToArray(),
				emailInfo.BlockName,
				emailInfo.RunStatus,
				emailInfo.IsSimulated);
		}
		#endregion

	}

	/// <summary>style of report generation post-run</summary>
	public enum PostRunReportGenerationOption
	{
		/// <summary>Generate report for current wellgroup</summary>
		CurrentWellGroup,
		/// <summary>Generate report for all wellgroups</summary>
		AllWellGroups,
		/// <summary>Do not generate any reports.</summary>
		None
	}

	/// <summary>
	/// Status after a run
	/// </summary>
	public class PostRunState
	{
		#region Member Data
		/// <summary>Displayable name of the block</summary>
		private string m_blockName;
		/// <summary>Status of the run: Completed, Failed or Cancelled</summary>
		private string m_runStatus;
		/// <summary>Full path to the .pcrd file generated after a run</summary>
		private string m_dataFile;
		/// <summary>Full path to the .pdf file generated after a run</summary>
		private List<string> m_reportFiles;
		/// <summary>The EmailInfo object</summary>
		private EmailInfo m_emailInfo;
		// Fix for Bug 9319 - store if the report has to be generated
		private PostRunReportGenerationOption m_GenerateReport = PostRunReportGenerationOption.None;
		private bool m_EmailGeneratedReports = true;
		#endregion

		#region Constructors and Destructor
		/// <summary>Constructor</summary>
		public PostRunState()
		{
		}
		#endregion

		#region Accessors
		/// <summary>Displayable name of the instrument</summary>
		public string BlockName
		{
			set { m_blockName = value; }
			get { return m_blockName; }
		}
		/// <summary>Status of the run: Completed, Failed or Cancelled</summary>
		public string RunStatus
		{
			set { m_runStatus = value; }
			get { return m_runStatus; }
		}
		/// <summary>Full path to the .pcrd file generated after a run</summary>
		public string DataFile
		{
			set { m_dataFile = value; }
			get { return m_dataFile; }
		}
		/// <summary>Full path to the .pdf file generated after a run</summary>
		public List<string> ReportFiles
		{
			set { m_reportFiles = value; }
			get { return m_reportFiles; }
		}
		/// <summary>The EmailInfo object</summary>
		public EmailInfo EmailInfoObj
		{
			set { m_emailInfo = value; }
			get { return m_emailInfo; }
		}
		// Fix for Bug 9319 - store if the report has to be generated
		/// <summary>Should the report be generated for the data file created?</summary>
		public PostRunReportGenerationOption GenerateReport
		{
			get { return this.m_GenerateReport; }
			set { this.m_GenerateReport = value; }
		}
		/// <summary>Whether to email the generated reports</summary>
		public bool EmailGeneratedReports
		{
			get { return m_EmailGeneratedReports; }
			set { m_EmailGeneratedReports = value; }
		}
		#endregion

	}

	/// <summary>
	/// All data needed to send email
	/// </summary>
	public class EmailInfo
	{
		#region Constructors and Destructor
		/// <summary>
		/// 
		/// </summary>
		public EmailInfo() { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="isSendMail"></param>
		public EmailInfo(bool isSendMail)
		{
			m_isSendMail = isSendMail;
		}
		#endregion

		#region Member Data
		// Standard email properties
		private string m_sendTo;
		private string m_sendCC;
		private string m_sendBCC;
		private string m_subject;
		private string m_message;
		private List<string> m_attachments;
		private bool m_isBodyHtml;

		// Additional properties
		private string m_blockName;
		private string m_runStatus;
		private bool m_isSimulated;
		private bool m_isSendMail;
		private bool m_isAttachDataFile;
		private bool m_isAttachReportFile;
		#endregion

		#region Accessors
		/// <summary>Email address to send to</summary>
		public string SendTo
		{
			set { m_sendTo = value; }
			get { return m_sendTo; }
		}
		/// <summary>Email address to send as CC</summary>
		public string SendCC
		{
			set { m_sendCC = value; }
			get { return m_sendCC; }
		}
		/// <summary>Email address to send as BCC</summary>
		public string SendBCC
		{
			set { m_sendBCC = value; }
			get { return m_sendBCC; }
		}
		/// <summary>Email subject line</summary>
		public string Subject
		{
			set { m_subject = value; }
			get { return m_subject; }
		}
		/// <summary>Email body message</summary>
		public string Message
		{
			set { m_message = value; }
			get { return m_message; }
		}
		/// <summary>Attachments to email</summary>
		public List<string> Attachments
		{
			set { m_attachments = value; }
			get { return m_attachments; }
		}
		/// <summary>Is the body message in Html format</summary>
		public bool isBodyHtml
		{
			set { m_isBodyHtml = value; }
			get { return m_isBodyHtml; }
		}

		/// <summary>Block name</summary>
		public string BlockName
		{
			set { m_blockName = value; }
			get { return m_blockName; }
		}
		/// <summary>Status of the run after completed</summary>
		public string RunStatus
		{
			set { m_runStatus = value; }
			get { return m_runStatus; }
		}
		/// <summary>Is simulation mode</summary>
		public bool IsSimulated
		{
			set { m_isSimulated = value; }
			get { return m_isSimulated; }
		}
		/// <summary>Is user selects to send email or not</summary>
		public bool IsSendMail
		{
			set { m_isSendMail = value; }
			get { return m_isSendMail; }
		}
		/// <summary>Is user selects to attach Data File or not</summary>
		public bool IsAttachDataFile
		{
			set { m_isAttachDataFile = value; }
			get { return m_isAttachDataFile; }
		}
		/// <summary>Is user selects to attach Report File or not</summary>
		public bool IsAttachReportFile
		{
			set { m_isAttachReportFile = value; }
			get { return m_isAttachReportFile; }
		}
		#endregion
	}
}
