import jenkins.model.*
import hudson.security.*

// Get environment variables.
def env = System.getenv()

// Change location configuration for admin address and Jenkins URL.
// Currently this will override settings changed via Jenkins UI.
// TODO: Investigate whether EMail is required at first setup and URL can be changed later.
def jenkinsLocationConfiguration = JenkinsLocationConfiguration.get()
jenkinsLocationConfiguration.setAdminAddress(env.JENKINS_EMAIL)
jenkinsLocationConfiguration.setUrl(env.JENKINS_URL)
jenkinsLocationConfiguration.save()

def jenkins = Jenkins.getInstance()

// Set security realm if not set.
if (!(jenkins.getSecurityRealm() instanceof HudsonPrivateSecurityRealm)) {
  jenkins.setSecurityRealm(new HudsonPrivateSecurityRealm(false))
}
// Set auth strategy if not set.
if (!(jenkins.getAuthorizationStrategy() instanceof GlobalMatrixAuthorizationStrategy)) {
  jenkins.setAuthorizationStrategy(new GlobalMatrixAuthorizationStrategy())
}

// Create default admin user if not exists.
def currentUsers = jenkins.getSecurityRealm().getAllUsers().collect { it.getId() }
if (!(env.JENKINS_USER in currentUsers)) {
  def user = jenkins.getSecurityRealm().createAccount(env.JENKINS_USER, env.JENKINS_PASS)
  user.save()
  // Set Administer permissions.
  jenkins.getAuthorizationStrategy().add(Jenkins.ADMINISTER, env.JENKINS_USER)
}

jenkins.save()